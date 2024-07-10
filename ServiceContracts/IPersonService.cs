using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Data;

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
        /// <summary>
        /// it sorts the list of persons based on the sort by and sort order
        /// </summary>
        /// <param name="persons">The list to be sorted</param>
        /// <param name="sortBy">sorting based on it</param>
        /// <param name="sortOrder">sort the list (ascending or descending)</param>
        /// <returns></returns>
        List<PersonResponse> GetSortedPersons(List <PersonResponse> persons, string sortBy,  SortOrderOptions sortOrder);
        /// <summary>
        /// updates the person based on personID in the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>returns object of person response</returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? request);
        /// <summary>
        /// Deletes the person based on personID
        /// </summary>
        /// <param name="PersonID"></param>
        /// <returns>returns true if the person is deleted successfully otherwise returns false </returns>
        bool DeletePerson(Guid? PersonID);
    }
}
