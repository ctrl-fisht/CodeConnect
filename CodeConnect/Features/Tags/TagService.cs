using AutoMapper;
using CodeConnect.Dto;
using CodeConnect.Data;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;

namespace CodeConnect.Features.Tags;

public class TagService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public TagService(IUserRepository userRepository, IMapper mapper, AppDbContext context)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<TagDto>> GetTagsList()
    {
        var tags = await _context.Tags.OrderBy(t => t.TagId).ToListAsync();
        return _mapper.Map<List<TagDto>>(tags);
    }
}
