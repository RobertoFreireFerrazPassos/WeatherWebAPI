﻿namespace Weather.Application.Map;

public class ConfigurationAppMapping : Profile
{
    public ConfigurationAppMapping()
    {
        CreateMap<RegistrationRequest, RegistrationDto>();
        CreateMap<RegistrationDto, User>();
    }
}
