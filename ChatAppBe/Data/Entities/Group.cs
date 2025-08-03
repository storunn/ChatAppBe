namespace ChatAppBe.Data.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<GroupMember> Members { get; set; } = new();

    }
}
