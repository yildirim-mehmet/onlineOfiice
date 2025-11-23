using System.Collections.Generic;

namespace OnlineOfficeWeb.Models {
    public class PowerPointStructureModel {
        public string FileName { get; set; } = "";
        public List<PowerPointSlideModel> Slides { get; set; } = new();
    }

    public class PowerPointSlideModel {
        public int Index { get; set; }
        public string? LayoutName { get; set; }
        public string? BackgroundType { get; set; }

        public List<PowerPointTextBoxModel> TextBoxes { get; set; } = new();
        public List<PowerPointShapeModel> Shapes { get; set; } = new();
        public List<PowerPointPictureModel> Pictures { get; set; } = new();
    }

    public class PowerPointTextBoxModel {
        public string Text { get; set; } = "";
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public string FontName { get; set; } = "";
        public double FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public string ColorHex { get; set; } = "";
    }

    public class PowerPointShapeModel {
        public string ShapeType { get; set; } = "";
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class PowerPointPictureModel {
        public string ImageType { get; set; } = "";
        public string FileName { get; set; } = "";
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
