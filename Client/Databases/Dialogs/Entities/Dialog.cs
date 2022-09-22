using System.ComponentModel.DataAnnotations;
namespace WebClient.Databases.Dialogs.Entities
{
    public class Dialog
    {
        [Key]
        public string Companion { get; init; }
        public int LocalId { get; init; }
        public int ActiveDecryptId { get; init; }
        public virtual List<DialogSegment> Segments { get; } = new();
        public string ParticipantId { get; init; }
        public virtual DialogPaticipant Participant { get; set; }
    }
}
