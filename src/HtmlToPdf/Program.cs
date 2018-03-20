using System;
using Bogus;
using DynamicTables;

namespace HtmlToPdf {
    class Data {
        public string User { set; get; }
        public string Email { set; get; }
        public string ZipCode { set; get; }
        public string City { set; get; }
    }

    class Program {
        public static int Main(string[] args) {
            var fake = new Faker<Data>()
                .RuleFor(x => x.User, f => f.Person.UserName)
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.ZipCode, f => f.Address.ZipCode(null))
                .RuleFor(x => x.City, f => f.Address.City());

            var rs = fake.Generate(20);
            DynamicTable.From(rs).Write();
            return 0;
        }
    }
}
