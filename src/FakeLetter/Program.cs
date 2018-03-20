using System;
using System.IO;
using Bogus;
using DynamicTables;
using System.Linq;
using PS = StartProcess.Processor;

namespace HtmlToPdf {
    class Data {
        public string User { set; get; }
        public string Email { set; get; }
        public string ZipCode { set; get; }
        public string City { set; get; }
    }

    class Program {
        public static int Main(string[] args) {

            if (args.Length != 2 && File.Exists(args[0])) {
                Console.WriteLine("> invalid arguments");
                return -1;
            }

            if (!Int32.TryParse(args[1], out var count)) {
                Console.WriteLine("> invalid arguments");
                return -1;
            }

            var template = args[0];
            var outputDir = "outputs";

            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

            var fake = new Faker<Data>()
                .RuleFor(x => x.User, f => f.Person.UserName)
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.ZipCode, f => f.Address.ZipCode(null))
                .RuleFor(x => x.City, f => f.Address.City());

            var datas = fake.Generate(count);
            var templateName = Path.GetFileName(template);

            var md = File.ReadAllText(template);
            var newMds = datas.Select((item, i) => {
                var newMd =
                 md.Replace("{userName}", item.User)
                 .Replace("{email}", item.Email)
                 .Replace("{zipCode}", item.ZipCode)
                 .Replace("{city}", item.City);

                var fileName = $"{i.ToString("D2")}-{templateName}";
                var tempFile = Path.Combine(Path.GetTempPath(), fileName);
                var outputFile = Path.Combine("outputs", Path.ChangeExtension(fileName, ".pdf"));

                File.WriteAllText(tempFile, newMd);

                PS.StartProcess($"pandoc {tempFile} -o {outputFile} --pdf-engine=pdflatex");

                File.Delete(tempFile);
                return new { Output = outputFile };
            });

            DynamicTable.From(newMds).Write();
            return 0;
        }
    }
}
