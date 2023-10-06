namespace Viabilidade.Domain.Entities.Alert
{
    public class FavoriteAlertEntity : BaseEntity
    {
        public Guid? UserId { get; set; }
        public int? RuleId { get; set; }
        public virtual RuleEntity Rule { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Active { get; set; }
    }
}
