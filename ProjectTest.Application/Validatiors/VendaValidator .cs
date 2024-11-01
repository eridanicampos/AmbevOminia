using FluentValidation;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Common;
using System;
using System.Linq;

namespace ProjectTest.Application.Validators
{
    public class VendaValidator : AbstractValidator<Venda>
    {
        private readonly IUnitOfWork _uow;

        public VendaValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(v => v.DataVenda)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .WithMessage("A data da venda não pode ser superior a um ano atrás.")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("A data da venda não pode estar no futuro.");

            RuleFor(v => v.Filial)
                .NotEmpty()
                .WithMessage("A filial é obrigatória.");

            RuleFor(v => v.Itens)
                .NotNull()
                .Must(itens => itens.Any())
                .WithMessage("A venda deve conter pelo menos um item.");

            RuleFor(v => v)
                .Must(v => v.ValorTotal == v.Itens.Where(i => !i.Cancelada).Sum(i => i.ValorTotalItem))
                .WithMessage("O valor total da venda não corresponde à soma dos itens.");

            RuleForEach(v => v.Itens)
                .SetValidator(new ItemVendaValidator(_uow));

            RuleFor(v => v.Cancelada)
                .Must(cancelada => !cancelada)
                .WithMessage("Não é possível modificar uma venda que já foi cancelada.");
        }
    }

    public class ItemVendaValidator : AbstractValidator<Domain.Entities.ItemVenda>
    {
        private readonly IUnitOfWork _uow;

        public ItemVendaValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(i => i.ProdutoId)
                .NotEmpty()
                .WithMessage("O ProdutoId é obrigatório.");

            RuleFor(i => i.NomeProduto)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório.");

            RuleFor(i => i.ValorUnitario)
                .GreaterThan(0)
                .WithMessage("O valor unitário do produto deve ser maior que zero.");

            RuleFor(i => i.Quantidade)
                .GreaterThan(0)
                .WithMessage("A quantidade deve ser maior que zero.")
                .LessThanOrEqualTo(20)
                .WithMessage("Não é permitido vender acima de 20 itens iguais.");

            RuleFor(i => i)
                .Must(i => AplicarDesconto(i))
                .WithMessage("Desconto inválido para a quantidade de itens.");
        }

        private bool AplicarDesconto(Domain.Entities.ItemVenda item)
        {
            if (item.Quantidade >= 4 && item.Quantidade < 10)
            {
                return item.DescontoValorUnitario == 0.10m;
            }
            else if (item.Quantidade >= 10 && item.Quantidade <= 20)
            {
                return item.DescontoValorUnitario == 0.20m;
            }

            return (item.DescontoValorUnitario == null || item.Quantidade < 4); 
        }
    }
}
