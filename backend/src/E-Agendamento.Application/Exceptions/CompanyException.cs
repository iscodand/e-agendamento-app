namespace E_Agendamento.Application.Exceptions
{
    public class CompanyException : Exception
    {
        public CompanyException() : base("Você não tem permissão para executar essa operação")
        {

        }
    }
}