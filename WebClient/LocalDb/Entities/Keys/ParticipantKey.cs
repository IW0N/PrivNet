using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.LocalDb.Entities.Keys
{
    public class ParticipantKey:AesKey
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public ChatParticiapnt Owner { get; init; }
       
        public ParticipantKey(byte[] Key, byte[] IV) : base(Key, IV) { }
    }
}
