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
        public string Phone { set; get; }
        public DateTime Date1 { set; get; }
        public DateTime Date2 { set; get; }
    }

    class Program {
        public static (bool, string, int) Validate(string[] args) {
            if (args.Length != 2 && File.Exists(args[0])) {
                Console.WriteLine("> invalid arguments");
                return (false, "", 0);
            }

            if (!Int32.TryParse(args[1], out var count)) {
                Console.WriteLine("> invalid arguments");
                return (false, "", 0);
            }

            return (true, args[0], count);
        }

        private static string CreateOutputDir() {
            var outputDir = "outputs";
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
            return outputDir;
        }

        private static string ProcessMd(string outputFile, string md) {
            var tempFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(outputFile) + ".md");
            File.WriteAllText(tempFile, md);
            Console.WriteLine($@"> creating ""{outputFile}""");

            PS.StartProcess($"pandoc {tempFile} -o {outputFile} --pdf-engine=pdflatex");
            File.Delete(tempFile);
            return outputFile;
        }

        public static int Main(string[] args) {
            var (ok, template, count) = Validate(args);
            if (!ok) return -1;

            var outputDir = CreateOutputDir();
            var fake = new Faker<Data>()
                .RuleFor(x => x.User, f => f.Person.UserName)
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.ZipCode, f => f.Address.ZipCode(null))
                .RuleFor(x => x.Phone, f => f.Person.Phone)
                .RuleFor(x => x.Date1, f => f.Date.Past(1))
                .RuleFor(x => x.Date2, f => f.Date.Past(10))
                .RuleFor(x => x.City, f => f.Address.City());

            var datas = fake.Generate(count);
            var templateName = Path.GetFileName(template);

            var md = File.ReadAllText(template);
            var newMds = datas.Select((item, i) => {
                var newMd =
                 md.Replace("{userName}", item.User)
                 .Replace("{email}", item.Email)
                 .Replace("{zipCode}", item.ZipCode)
                 .Replace("{date1}", item.Date1.ToString("dd-MM-yyyy"))
                 .Replace("{data2}", item.Date2.ToString("dd-MM-yyyy"))
                 .Replace("{phone}", item.Phone)
                 .Replace("{city}", item.City);

                var fileName = $"{(i + 1).ToString("D2")}-{templateName}";
                var outputFile = Path.Combine(outputDir, Path.ChangeExtension(fileName, ".pdf"));
                ProcessMd(outputFile, newMd);
                return new { Output = outputFile };
            });

            DynamicTable.From(newMds).Write();
            return 0;
        }
    }
}
