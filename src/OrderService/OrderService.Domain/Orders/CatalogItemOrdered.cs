namespace OrderService.Domain.Orders;

public class CatalogItemOrdered
{
    public int CatalogItemId { get; private set; }
    public string ProductName { get; private set; }
    public string PictureUri { get; private set; }

#pragma warning disable CS8618
    private CatalogItemOrdered() { }

    public CatalogItemOrdered(int catalogItemId, string productName, string pictureUri)
    {
        // TODO: Verificar como validar da melhor forma !!!
        //Guard.Against.OutOfRange(catalogItemId, nameof(catalogItemId), 1, int.MaxValue);
        //Guard.Against.NullOrEmpty(productName, nameof(productName));
        //Guard.Against.NullOrEmpty(pictureUri, nameof(pictureUri));

        CatalogItemId = catalogItemId;
        ProductName = productName;
        PictureUri = pictureUri;
    }
}