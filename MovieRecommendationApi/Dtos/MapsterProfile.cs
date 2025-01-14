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
                .Map(dest => dest.ImdbId, src => src.ImdbId)
                .Map(dest => dest.TmdbId, src => src.TmdbId)
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
                .Map(dest => dest.MovieCredit, src => src.MovieCredit);

            config.NewConfig<Movie, MovieDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ImdbId, src => src.ImdbId)
                .Map(dest => dest.TmdbId, src => src.TmdbId)
                .Map(dest => dest.Adult, src => src.Adult)
                .Map(dest => dest.BackdropPath, src => src.BackdropPath)
                .Map(dest => dest.BelongsToCollection, src => src.BelongsToCollection)
                .Map(dest => dest.Budget, src => src.Budget)
                .Map(dest => dest.Categories, src => src.Categories)
                .Map(dest => dest.Genres, src => src.Genres)
                .Map(dest => dest.Homepage, src => src.Homepage)
                .Map(dest => dest.OriginCountry, src => src.OriginCountry)
                .Map(dest => dest.OriginalLanguage, src => src.OriginalLanguage)
                .Map(dest => dest.OriginalTitle, src => src.OriginalTitle)
                .Map(dest => dest.Overview, src => src.Overview)
                .Map(dest => dest.Popularity, src => src.Popularity)
                .Map(dest => dest.PosterPath, src => src.PosterPath)
                .Map(dest => dest.ProductionCompanies, src => src.ProductionCompanies)
                .Map(dest => dest.ProductionCountries, src => src.ProductionCountries)
                .Map(dest => dest.ReleaseDate, src => src.ReleaseDate)
                //.Map(dest => dest.Revenue, src => src.Revenue)
                .Map(dest => dest.Runtime, src => src.Runtime)
                .Map(dest => dest.SpokenLanguages, src => src.SpokenLanguages)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.Tagline, src => src.Tagline)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Video, src => src.Video)
                .Map(dest => dest.VoteAverage, src => src.VoteAverage)
                .Map(dest => dest.VoteCount, src => src.VoteCount);

            config.NewConfig<BelongsToCollection, BelongsToCollectionDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.PosterPath, src => src.PosterPath)
                .Map(dest => dest.BackdropPath, src => src.BackdropPath);

            config.NewConfig<Genre, GenreDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name);

            config.NewConfig<ProductionCompany, ProductionCompanyDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.LogoPath, src => src.LogoPath)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.OriginCountry, src => src.OriginCountry);

            config.NewConfig<ProductionCountry, ProductionCountryDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.IsoCode, src => src.IsoCode)
                .Map(dest => dest.Name, src => src.Name);


            config.NewConfig<SpokenLanguage, SpokenLanguageDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.IsoCode, src => src.IsoCode)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.EnglishName, src => src.EnglishName);

            config.NewConfig<MovieCredit, MovieCreditDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Cast, src => src.Cast);

            config.NewConfig<Credit, CreditDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Cast, src => src.Cast);

            config.NewConfig<Review, ReviewDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.MovieId, src => src.MovieId)
                .Map(dest => dest.Content, src => src.Content)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.Url, src => src.Url)
                .Map(dest => dest.Url, src => src.Url)
                .Map(dest => dest.Url, src => src.Url)
                .Map(dest => dest.Rating, src => src.Rating)
                .Map(dest => dest.AuthorDetails, src => src.User);

            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Username, src => src.Email)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.AvatarPath, src => src.AvatarPath);
        }
    }
}
