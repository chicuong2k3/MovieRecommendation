using Mapster;
using MovieRecommendationApi.Models;

namespace MovieRecommendationApi.Dtos
{
    public class MapsterProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Person, PersonDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Adult, src => src.Adult)
                .Map(dest => dest.AlsoKnownAs, src => src.AlsoKnownAs)
                .Map(dest => dest.Biography, src => src.Biography)
                .Map(dest => dest.Birthday, src => src.Birthday)
                .Map(dest => dest.Deathday, src => src.Deathday)
                .Map(dest => dest.Gender, src => src.Gender)
                .Map(dest => dest.Homepage, src => src.Homepage)
                .Map(dest => dest.KnownForDepartment, src => src.KnownForDepartment)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.PlaceOfBirth, src => src.PlaceOfBirth)
                .Map(dest => dest.Popularity, src => src.Popularity)
                .Map(dest => dest.ProfilePath, src => src.ProfilePath)
                .Map(dest => dest.CreditId, src => src.MovieCredits != null ? src.MovieCredits.Id : (int?)null);
        }
    }
}
