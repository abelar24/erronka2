namespace TiketBAI
{
    public class Saltzaile
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Saltzaile(int id, string code, string name)
        {
            Id = id;
            Code = code;
            Name = name;
        }
    }
}