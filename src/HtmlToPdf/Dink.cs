using System;
using DinkToPdf;

namespace HtmlToPdf {
    public class Dink {
        public static void Test() {
            Console.WriteLine("Hello, world!");
            var doc = new HtmlToPdfDocument() {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10 },
                Out = @"test.pdf",
            },
                Objects = {
                    new ObjectSettings() {
                        Page = "http://google.com",
                    },
                }
            };

            var converter = new BasicConverter(new PdfTools());
            converter.Convert(doc);
        }
    }
}