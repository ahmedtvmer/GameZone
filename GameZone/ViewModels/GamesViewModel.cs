﻿using GameZone.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.ViewModels
{
    public class CreateGameViewModel : GameBaseViewModel
    {
        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [MaxFileSize(FileSettings.MaxFileSizeInMB)]
        public IFormFile Cover { get; set; } = default!;
    }
}
