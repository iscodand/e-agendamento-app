namespace E_Agendamento.Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalItems = 0)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            Data = data;
            Message = "Dados recuperados com sucesso";
            Succeeded = true;
            Errors = null;
        }
    }
}