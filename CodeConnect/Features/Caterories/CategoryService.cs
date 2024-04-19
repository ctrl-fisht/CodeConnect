using AutoMapper;
using CodeConnect.CommonDto;
using CodeConnect.Data;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CodeConnect.Features.Caterories;

public class CategoryService
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CategoryService(IUserRepository userRepository, AppDbContext context, IMapper mapper)
    {
        _userRepository = userRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetCategoryList()
    {
       var categories =  await _context.Categories.OrderBy(c => c.CategoryId).ToListAsync();
       return _mapper.Map<List<CategoryDto>>(categories);
    }
}
