using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "ID")]
        public int ProductId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название товара")]
        public string Name { get; set; }

        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Пожалуйста, укажите имя производителя")]
        public string Brand { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание товара")]
        public string Description { get; set; }

        [Display(Name = "Тип товара")]
        [Required(ErrorMessage = "Пожалуйста, укажите тип продукта")]
        public string Type { get; set; }

        [Display(Name = "Пикча товара")]
        public byte[] Image { get; set; }

        [Display(Name = "Цена (Тг)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение цены")]
        public decimal Price { get; set; }
    }
}
