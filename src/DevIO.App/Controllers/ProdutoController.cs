using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevIO.App.Data;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.IO.Pipelines;

namespace DevIO.App.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepository _repository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutoController(IProdutoRepository repository, IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _repository = repository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }


        // GET: Produto
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _repository.ObterProdutosFornecedores()));
        }

        // GET: Produto/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        // GET: Produto/Create
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await GetFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        // POST: Produto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await GetFornecedores(produtoViewModel);

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            var prefix = Guid.NewGuid() + "_";

            if (!await ImagemUpload(produtoViewModel.ImagemUpload, prefix))
                return View(produtoViewModel);

            produtoViewModel.Imagem = prefix + produtoViewModel.ImagemUpload.FileName;

            await _repository.Add(_mapper.Map<Produto>(produtoViewModel));
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Produto/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
                return NotFound();

            var produtoAtualizado = await ObterProduto(id);
            produtoViewModel.Fornecedor = produtoAtualizado.Fornecedor;
            produtoViewModel.Imagem = produtoAtualizado.Imagem;

            if (!ModelState.IsValid)
            {
                return View(produtoViewModel);
            }

            if(produtoViewModel.ImagemUpload != null)
            {
                var prefix = Guid.NewGuid() + "_";

                if (!await ImagemUpload(produtoViewModel.ImagemUpload, prefix) ||
                    !DeletarImagem(produtoViewModel.Imagem))
                {
                    return View(produtoViewModel);
                }
 
                produtoAtualizado.Imagem = prefix + produtoViewModel.ImagemUpload.FileName;
            }

            produtoAtualizado.Nome = produtoViewModel.Nome;
            produtoAtualizado.Descricao = produtoViewModel.Descricao;
            produtoAtualizado.Valor = produtoViewModel.Valor;
            produtoAtualizado.Ativo = produtoViewModel.Ativo;

            await _repository.Edit(_mapper.Map<Produto>(produtoAtualizado));

            return RedirectToAction(nameof(Index));
        }

        // GET: Produto/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            await _repository.Remove(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _repository.ObterProdutoFornecedor(id));
            produtoViewModel.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll());
            return produtoViewModel;
        }

        private async Task<ProdutoViewModel> GetFornecedores(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll());
            return produtoViewModel;
        }

        private async Task<bool> ImagemUpload(IFormFile file, string prefix)
        {
            if (file.Length <= 0)
                return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", prefix + file.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome");
                return false;
            }

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }

        private bool DeletarImagem(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }

            return false;
        }
    }
}
