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
    }
}
