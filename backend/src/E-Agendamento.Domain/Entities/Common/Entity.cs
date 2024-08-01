namespace E_Agendamento.Domain.Entities.Common
{
    public abstract class Entity
    {
        public string Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
