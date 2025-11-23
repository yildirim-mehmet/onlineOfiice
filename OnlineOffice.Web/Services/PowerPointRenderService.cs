using Microsoft.AspNetCore.Hosting;
using OfficeIMO.PowerPoint;
using System.Collections.Generic;
using System.IO;






namespace OnlineOfficeWeb.Services {
    public class PowerPointRenderService {

        private readonly IWebHostEnvironment _env;

        public PowerPointRenderService(IWebHostEnvironment env) {
            _env = env;
        }

        /// <summary>
        /// PPTX dosyasýndaki tüm slaytlarý PNG olarak üretir.
        /// </summary>
        public List<string> RenderAllSlides(string filePath, string outputDirName) {
            var outputDir = Path.Combine(_env.WebRootPath, "ppt", outputDirName);

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            using var ppt = PowerPointDocument.Load(filePath);

            var pngList = new List<string>();

            int index = 1;
            foreach (var slide in ppt.Slides) {

                string pngName = $"slide_{index}.png";
                string pngPath = Path.Combine(outputDir, pngName);

                // OfficeIMO PowerPoint API
                var img = slide.SaveAsImage();

                using var fs = new FileStream(pngPath, FileMode.Create);
                img.Save(fs);

                pngList.Add($"/ppt/{outputDirName}/{pngName}");

                index++;
            }

            return pngList;
        }
    }
}
