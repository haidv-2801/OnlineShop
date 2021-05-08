namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reply")]
    public partial class Reply
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Text { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? UserId { get; set; }

        public long? CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public virtual User User { get; set; }
    }
}
