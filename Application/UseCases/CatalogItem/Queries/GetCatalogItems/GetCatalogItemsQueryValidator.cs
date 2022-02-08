using FluentValidation;

namespace Application.UseCases.CatalogItem.Queries.GetCatalogItems
{
    class GetCatalogItemsQueryValidator : AbstractValidator<GetCatalogItemsQuery>
    {
        public GetCatalogItemsQueryValidator()
        {
            RuleFor(q => q.CatalogId).NotEmpty();
        }
    }
}
