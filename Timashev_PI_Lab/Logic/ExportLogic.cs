using Timashev_PI_Lab.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using Timashev_PI_Lab.HelperModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;

namespace Timashev_PI_Lab.Logic
{
    public class ExportLogic
    {
        static string exportDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\Export\\";
        Database context;

        public ExportLogic(Database _context)
        {
            context = _context;
        }

        public static string CreateDoc(WordInfo info, List<TechCard> techCards)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(exportDirectory + info.FileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body docBody = mainPart.Document.AppendChild(new Body());

                docBody.AppendChild(CreateParagraph(new WordParagraph
                {
                    Texts = new List<string> { info.Title + " от " + DateTime.Now.ToString("yyyy-MM-dd") },
                    TextProperties = new WordParagraphProperties
                    {
                        Bold = true,
                        Size = "28",
                        JustificationValues = JustificationValues.Center
                    }
                }));
                docBody.AppendChild(CreateParagraph(new WordParagraph
                {
                    Texts = new List<string> { "Сотрудник | Работы(шт) | Оклад(руб) | Зарплата(руб)", "" },
                    TextProperties = new WordParagraphProperties
                    {
                        Bold = true,
                        Size = "24",
                        JustificationValues = JustificationValues.Both
                    }
                }));
                docBody.AppendChild(CreateSectionProperties());
                wordDocument.MainDocumentPart.Document.Save();
            }
            return exportDirectory + info.FileName;
        }

        private static SectionProperties CreateSectionProperties()
        {
            SectionProperties properties = new SectionProperties();
            PageSize pageSize = new PageSize { Orient = PageOrientationValues.Portrait };

            properties.AppendChild(pageSize);

            return properties;
        }

        private static Paragraph CreateParagraph(WordParagraph paragraph)
        {
            if (paragraph != null)
            {
                Paragraph docParagraph = new Paragraph();

                docParagraph.AppendChild(CreateParagraphProperties(paragraph.TextProperties));

                foreach (var run in paragraph.Texts)
                {
                    Run docRun = new Run();
                    RunProperties properties = new RunProperties();
                    properties.AppendChild(new FontSize { Val = paragraph.TextProperties.Size });

                    if (!run.StartsWith(" - ") && paragraph.TextProperties.Bold)
                    {
                        properties.AppendChild(new Bold());
                    }

                    docRun.AppendChild(properties);
                    docRun.AppendChild(new Text { Text = run, Space = SpaceProcessingModeValues.Preserve });
                    docParagraph.AppendChild(docRun);
                }

                return docParagraph;
            }

            return null;
        }

        private static ParagraphProperties CreateParagraphProperties(WordParagraphProperties paragraphProperties)
        {
            if (paragraphProperties != null)
            {
                ParagraphProperties properties = new ParagraphProperties();

                properties.AppendChild(new Justification() { Val = paragraphProperties.JustificationValues });
                properties.AppendChild(new SpacingBetweenLines { LineRule = LineSpacingRuleValues.Auto });
                properties.AppendChild(new Indentation());

                ParagraphMarkRunProperties paragraphMarkRunProperties = new ParagraphMarkRunProperties();

                if (!string.IsNullOrEmpty(paragraphProperties.Size))
                {
                    paragraphMarkRunProperties.AppendChild(new FontSize { Val = paragraphProperties.Size });
                }

                if (paragraphProperties.Bold)
                {
                    paragraphMarkRunProperties.AppendChild(new Bold());
                }

                properties.AppendChild(paragraphMarkRunProperties);

                return properties;
            }

            return null;
        }
    }
}
