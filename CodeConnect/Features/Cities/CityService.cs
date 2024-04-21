using AutoMapper;
using CodeConnect.Dto;
using CodeConnect.Data;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;

namespace CodeConnect.Features.Cities;

public class CityService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public CityService(IUserRepository userRepository, IMapper mapper, AppDbContext context)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<CityDto>> GetCityList()
    {
        var cities = await _context
            .Cities
            .OrderBy(c => c.CityId)
            .ToListAsync();
        return _mapper.Map<List<CityDto>>(cities);
    }
}
