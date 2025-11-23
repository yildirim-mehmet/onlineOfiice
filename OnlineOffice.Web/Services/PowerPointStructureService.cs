using OfficeIMO.PowerPoint;
using OnlineOfficeWeb.Models;

namespace OnlineOfficeWeb.Services {
    public class PowerPointStructureService {
        public PowerPointStructureModel ExtractStructure(string filePath) {
            var model = new PowerPointStructureModel {
                FileName = Path.GetFileName(filePath)
            };

            // OfficeIMO sunumu yükle
            PowerPointPresentation presentation = PowerPointPresentation.Open(filePath);

            int index = 1;

            foreach (var slide in presentation.Slides) {
                var slideModel = new PowerPointSlideModel {
                    Index = index++,
                    LayoutName = slide.Layout?.Name,
                    BackgroundType = slide.Background?.FillStyle?.ToString()
                };

                // 📌 TEXTBOX’LARI OKU
                foreach (var textBox in slide.TextBoxes) {
                    slideModel.TextBoxes.Add(new PowerPointTextBoxModel {
                        Text = textBox.Text,
                        Left = textBox.Left,
                        Top = textBox.Top,
                        Width = textBox.Width,
                        Height = textBox.Height,
                        FontName = textBox.FontName,
                        FontSize = textBox.FontSize,
                        Bold = textBox.Bold,
                        Italic = textBox.Italic,
                        ColorHex = textBox.ColorHex
                    });
                }

                // 📌 SHAPE’LERİ OKU
                foreach (var shape in slide.Shapes) {
                    slideModel.Shapes.Add(new PowerPointShapeModel {
                        ShapeType = shape.ShapeType.ToString(),
                        Left = shape.Left,
                        Top = shape.Top,
                        Width = shape.Width,
                        Height = shape.Height
                    });
                }

                // 📌 RESİMLERİ OKU
                foreach (var picture in slide.Pictures) {
                    slideModel.Pictures.Add(new PowerPointPictureModel {
                        ImageType = picture.ImageType.ToString(),
                        FileName = picture.FileName,
                        Left = picture.Left,
                        Top = picture.Top,
                        Width = picture.Width,
                        Height = picture.Height
                    });
                }

                model.Slides.Add(slideModel);
            }

            return model;
        }
    }
}
