// Hager Fathy Mahmoud Ahmed;

//B1 Average on empty collection throws InvalidOperationException
//B2 insertion order not sorted
//B3 3


using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

const double GPAThreshold = 3.4;

List<Student> students = new(){
    new Student { FullName = "Yara Adel", YearOfStudy = 2, Gpa = 3.5 },
    new Student { FullName = "Omar Hesham", YearOfStudy = 3, Gpa = 2.8 },
    new Student { FullName = "Nada Samir", YearOfStudy = 1, Gpa = 3.9 },
    new Student { FullName = "Kareem Fouad", YearOfStudy = 4, Gpa = 3.2 }
};


List<Instructor> instructors = new(){
    new Instructor
    {
        FullName = "Hamdy",
        YearsOfExperience = 10,
        AssignedCourseName = "Web Development Using .NET"
    },
     new Instructor
    {
        FullName = "Mona Khalil",
        YearsOfExperience = 6,
        AssignedCourseName = "Database Fundamentals"
    }
};

List<Course> courses = new()
{
    new Course
    {
        CourseName = "Web Development Using .NET",
        Credits = 4
    },
    new Course
    {
        CourseName = "Database Fundamentals",
        Credits = 3
    }
};



Console.WriteLine($"Total Count: {students.Count()}");
Console.WriteLine($"Above GPA Threshold: {students.Count(s => s.Gpa >= GPAThreshold)}");
Console.WriteLine($"Average GPA: {students.Average(s => s.Gpa):F2}");  
Console.WriteLine($"Highest GPA: {students.Max(s => s.Gpa):F2}");  
Console.WriteLine($"Lowest GPA: {students.Min(s => s.Gpa):F2}");  
Console.WriteLine($"Any Below 2.0 ? : {students.Any(s => s.Gpa < 2.0)}"); 
Console.WriteLine($"All at or above 2.0: {students.All(s => s.Gpa >= 2.0)}");


//var noStudents = new list<Student>();
//Console.WriteLine($"Total Count : {noStudents.Count()}");
//Console.WriteLine($"Any Students ? : {noStudents.Any()}");
//Console.WriteLine($"Average of Empty List : {noStudents.Average(s => s.Gpa):F2}");


var noStudents = new List<Student>();
if(noStudents.Any())
Console.WriteLine("Average : {noStudents.Average(s => s.Gpa): F2}");
else
Console.WriteLine($"No Students to Calculate Average");


//Groups are not sorted, GroupBy makes the order groups are first encountered in the source sequence

var GroupedByYear = students.GroupBy(s => s.YearOfStudy);
foreach(var group in GroupedByYear){
    Console.WriteLine($"{group.Key} ({group.Count()} students) : {string.Join(", ", group.Select(s => s.FullName))}"); 
}

var groupedByCategory = students.GroupBy(s => s.Gpa >= GPAThreshold ? "Excellent" : "Developing");

foreach (var group in groupedByCategory){
    Console.WriteLine($"{group.Key} ({group.Count()} students): {string.Join(", ", group.Select(s => s.FullName))}");
}

//Added OrderBy(g => g.Key) to sort the groups by year
var sortedGroups = students
    .GroupBy(s => s.YearOfStudy)
    .OrderBy(g => g.Key);

foreach (var group in sortedGroups){
    Console.WriteLine($"Year {group.Key}: {string.Join(", ", group.Select(s => s.FullName))}");
}


var joinedMethod = instructors.Join(
    courses,
    inst => inst.AssignedCourseName,
    c => c.CourseName,
    (inst, c) => new { Instructor = inst.FullName, Course = c.CourseName, Credits = c.Credits })
    .ToList();
    foreach(var result in joinedMethod){
        Console.WriteLine($"{result.Instructor} teaches {result.Course} ({result.Credits} credits)");
    }


   var joinedQuery =(
    from inst in instructors
    join c in courses
        on inst.AssignedCourseName equals c.CourseName
    select new
    {
        Instructor = inst.FullName,
        Course = c.CourseName,
        Credits = c.Credits
    }
)
.ToList();

foreach (var result in joinedQuery)
{
    Console.WriteLine($"{result.Instructor} teaches {result.Course} ({result.Credits} credits)");
}

    var myInstructor = new Instructor{
        FullName = "Hager Fathy",  // Using real name
        YearsOfExperience = 3,
        AssignedCourseName = "Machine Learning"
    };

    instructors.Add(myInstructor);

    Console.WriteLine($"Instructors Count IN: {instructors.Count}");
    var joinWithMe = instructors.Join(
        courses,
        inst => inst.AssignedCourseName,
        c => c.CourseName,
        (inst, c) => new{Instructor = inst.FullName, Course = c.CourseName,Credits = c.Credits })
        .ToList();
        Console.WriteLine($"Join Rows OUT: {joinWithMe.Count}");
        instructors.Remove(myInstructor); 
        Console.WriteLine();

//Inner join silently drops non-matching rows "Hager Fathy" assigned to "Machine Learning" doesn't match any course so the row is excluded from results
//I would need a Left Outer Join 
//DefaultIfEmpty() or or an Outer Join

// Prediction 3 _ Correct
var TopStudent = students.Where(s => s.Gpa >= 3.0);

var layla = new Student{
    FullName = "Layla Mostafa",
    YearOfStudy = 2,
    Gpa = 3.7
};

students.Add(layla);

Console.WriteLine($"Count: {TopStudent.Count()}");

students.Remove(layla);
Console.WriteLine();


var TopStudentsDeferred = students.Where(s => s.Gpa > 3.0);
Console.WriteLine($"Count: {TopStudentsDeferred.Count()}"); 
foreach (var st in TopStudentsDeferred) { 

    Console.WriteLine($"{st.FullName}");
}

var avg1 = TopStudentsDeferred.Average(s => s.Gpa);  
Console.WriteLine($"Average: {avg1:F2}");

//Filtering runs 3 times

var TopStudentsImmediate = students.Where(s => s.Gpa > 3.0).ToList();  // Filter runs once
Console.WriteLine($"  Count: {TopStudentsImmediate.Count}");  // Use .Count property (not method)
foreach (var st in TopStudentsImmediate)  {
    Console.WriteLine($"{st.FullName}");
}

var avg2 = TopStudentsImmediate.Average(s => s.Gpa); 
Console.WriteLine($"Average: {avg2:F2}");

//Added .ToList() to force immediate execution, it becomes important in Part H when querying a database


var myTopNames = students
    .MyTopStudents()
    .OrderBy(s => s.FullName)
    .Select(s => s.FullName)
    .ToList();
foreach (var name in myTopNames)
{
    Console.WriteLine($"{name}");
}

//It's DEFERRED,  because Deferred execution means you filter IN SQL which is fast and efficient


Console.WriteLine("Step 1: Add Migration InitialCreate");
Console.WriteLine("Step 2: Read migration file");
Console.WriteLine("Step 3: Update Database");
Console.WriteLine("Step 4: Verify in SSMS");
Console.WriteLine();



using (var context = new StudentPortalContext())
{
   
    if (!context.Students.Any()){
    
        Console.WriteLine("Seeding database:");
        context.AddRange(students);
        context.SaveChanges();
        Console.WriteLine("Students seeded successfully");
    }
    else
    {
        Console.WriteLine("Database already seeded");
    }

    Console.WriteLine("Warm up chain against DATABASE:");
    var dbTopNames = context.Students
        .Where(s => s.Gpa > 3.0) 
        .OrderByDescending(s => s.Gpa) 
        .Select(s => s.FullName)  
        .ToList();  
    foreach (var name in dbTopNames)
    {
        Console.WriteLine($"{name}");
    }

   
    Console.WriteLine("Database Aggregates:");
    var avgGPA = context.Students.Average(s => s.Gpa);
    var totalCount = context.Students.Count();
    Console.WriteLine($"Average GPA: {avgGPA:F2}");
    Console.WriteLine($"Total students: {totalCount}");
}

//context.Students.Where(s => s.Gpa > 3.0).ToList(), Filtering happens in SQL
//context.Students.ToList().Where(s => s.Gpa > 3.0), Filtering happens in C#
//Used the first approach


public class Student{

    public int Id { get; set; }
    public string FullName { get; set; }
    public int YearOfStudy { get; set; }
    public double Gpa { get; set; }
    public string? Email { get; set; }  
}

public class Instructor{

    public int Id { get; set; }
    public string FullName { get; set; } 
    public int YearsOfExperience { get; set; }
    public string? AssignedCourseName { get; set; }
}

public class Course{

    public int Id { get; set; }
    public string CourseName { get; set; }
    public int Credits { get; set; }
}

public class StudentPortalContext : DbContext{

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }

     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=StudentPortal.db")
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .EnableSensitiveDataLogging();
    }
}

public static class StudentExtensions
{
    public static IEnumerable<Student> MyTopStudents(this IEnumerable<Student> students)
    {
        return students.Where(s => s.Gpa > 3.0);
    }
}







   
 