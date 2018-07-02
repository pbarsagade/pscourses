namespace WebAPI.Repository
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
