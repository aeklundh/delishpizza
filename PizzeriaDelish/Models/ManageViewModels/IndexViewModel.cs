using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public bool IsEmailConfirmed { get; set; }
        
        public string StatusMessage { get; set; }

        public ApplicationUser User { get; set; }
    }
}
