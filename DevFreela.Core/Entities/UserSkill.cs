namespace DevFreela.Core.Entities;

public class UserSkill : BaseEntity
{
    protected UserSkill() { }
    public UserSkill(int userId, int skillId) : base()
    {
        IdUser = userId;
        IdSkill = skillId;
    }
    public int IdUser { get; private set; }
    public User User { get; private set; }
    public int IdSkill { get; private set; }
    public Skill Skill { get; private set; }
}