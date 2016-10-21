namespace SmartParkAPI.Model.Common
{
    internal interface IEntity<T>
    {
        T Id { get; set; }
    }
}
