namespace Domain.Entity;

public class Password
{
    private bool IsPasswordValid(string password)
    {
        //todo логика проверки пароля, использовать регулярку 
        // Проверка на соответствие требованиям пароля
        //  необходимо реализовать эту проверку с использованием регулярных выражений или других методов.
        // Здесь предполагается, что у  уже есть метод IsPasswordValid, который проводит необходимые проверки
        // и возвращает true, если пароль соответствует требованиям, и false в противном случае.
        return true;
    }
    

}
    
    // // Проверка пароля
    // if (!IsStrongPassword(userDTO.Password))
    // {
    //     return BadRequest("Неверный формат пароля."); //400
    // }
    
    
    
    
    
    

    
    