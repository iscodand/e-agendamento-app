namespace E_Agendamento.Application.Wrappers
{
    public class Response<T>
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public ICollection<string> Errors { get; set; }

        public Response()
        {

        }

        public Response(string message, T data)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
    }
}