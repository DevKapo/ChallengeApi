using AutoMapper;
using ChallengeApi.DTOs;
using ChallengeApi.Entities;
using ChallengeApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Routing.Controllers;

[Route("odata/Productoras")]
[ApiController]
public class ProductoraController : ODataController
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductoraController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoraDto>>> GetAll()
    {
        var productoras = await _context.Productoras.ToListAsync();
        return Ok(_mapper.Map<List<ProductoraDto>>(productoras));
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<ProductoraDto>> GetById(int key)
    {
        var productora = await _context.Productoras.FindAsync(key);
        if (productora == null)
            return NotFound();

        return Ok(_mapper.Map<ProductoraDto>(productora));
    }

    [HttpPost]
    public async Task<ActionResult<ProductoraDto>> Create(ProductoraCrearDto dto)
    {
        var entity = _mapper.Map<Productora>(dto);
        _context.Productoras.Add(entity);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<ProductoraDto>(entity);
        return CreatedAtAction(nameof(GetById), new { key = resultDto.Id }, resultDto);
    }

    [HttpPut("{key}")]
    public async Task<IActionResult> Update(int key, ProductoraCrearDto dto)
    {
        var existing = await _context.Productoras.FindAsync(key);
        if (existing == null)
            return NotFound();

        _mapper.Map(dto, existing);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(int key)
    {
        var productora = await _context.Productoras.FindAsync(key);
        if (productora == null)
            return NotFound();

        _context.Productoras.Remove(productora);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
