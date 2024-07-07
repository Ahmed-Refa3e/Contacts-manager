using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonService
    {
        /// <summary>
        /// it creates a new person and add it to database 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>returns the person object</returns>
        PersonResponse AddPerson(PersonAddRequest? request);
        /// <summary>
        /// it returns all the persons as a list of person response objects
        /// </summary>
        /// <returns></returns>
        List<PersonResponse> GetAllPersons();
        /// <summary>
        /// it returns a person by person id
        /// </summary>
        /// <param name="PersonID"></param>
        /// <returns></returns>
        PersonResponse? GetPersonByPersonID(Guid? PersonID);

        /// <summary>
        /// it returns a list of persons based on the search criteria
        /// </summary>
        /// <param name="SerachBy">Search based on this parameter</param>
        /// <param name="SearchValue">it returns objects that contain this search value</param>
        /// <returns></returns>
        List<PersonResponse> GetFilteredPersons(string SearchBy, string SearchValue);
    }
}
