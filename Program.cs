// Hager Fathy Mahmoud Ahmed;
var nada = await context.Students.FirstAsync(s => s.FullName == "Nada Samir");
 Console.WriteLine($"Current GPA: {nada.Gpa}");

  nada.Gpa = 3.1; 
  Console.WriteLine($"Changed to: {nada.Gpa}");
  await context.SaveChangesAsync();
  Console.WriteLine($"Saved to database");


   //Difference is that EF tracks in memory but hasn't sent to database

   nada.Gpa = myGpa; 
   await context.SaveChangesAsync();
   Console.WriteLine($"SSMS value: {nada.Gpa}");

   //EF knew to update only Gpa by changing tracking

   var newStudent = new Student{
    FullName = "Hager Fathy",
    YearOfStudy = 2,
    Gpa = 3.1
    };
    
    Console.WriteLine($"Id before save: {newStudent.Id}");
    context.Students.Add(newStudent);
    await context.SaveChangesAsync();
    Console.WriteLine($"Id after save: {newStudent.Id}");

    newStudent.YearOfStudy = 3;
    await context.SaveChangesAsync();
    Console.WriteLine($"Updated in SSMS: Year is now 3");
   

    var DeleteID = newStudent.Id;
    context.Students.Remove(newStudent);
    await context.SaveChangesAsync();
    Console.WriteLine($"Deleted student id {DeleteID}");
    Console.WriteLine();

   // Remove() has no async because it only changes tracking state not database






   
 