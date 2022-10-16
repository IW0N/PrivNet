using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.LocalDb.Entities
{
    public class RSALock
    {
        [Key]
        public int Id { get; set; }
        public byte[] Key { get; init; }
        public byte[] Lock { get; init; }
        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        public LocalChat Chat { get; init; }
       
    }
}
