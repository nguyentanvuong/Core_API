using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class MultilTransactionReponse
    {
        [Required]
        public List<TransactionReponse> result { get; set; }
    }
}
