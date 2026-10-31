// Hager Fathy Mahmoud Ahmed;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

const double MyGpa = 3.1;

using var context = new StudentPortalContext();

await context.Database.MigrateAsync();


var nada = await context.Students
    .FirstOrDefaultAsync(s => s.FullName == "Nada Samir");

if (nada == null)cd ~/course1/ConsoleApp1{
    nada = new Student
    {
        FullName = "Nada Samir",
        YearOfStudy = 1,
        Gpa = 3.9
    };

    context.Students.Add(nada);

    await context.SaveChangesAsync();
}


   nada.Gpa = MyGpa; 
   await context.SaveChangesAsync();
   Console.WriteLine($"SSMS value: {nada.Gpa}");


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
    Console.WriteLine($"Updated Year is now 3");
   

    var DeleteID = newStudent.Id;
    context.Students.Remove(newStudent);
    await context.SaveChangesAsync();
    Console.WriteLine($"Deleted student id {DeleteID}");
    Console.WriteLine();

 

try{
    var Student1 = new Student{
          FullName = null,
          YearOfStudy = 1,
          Gpa = 3.0
    };
    context.Students.Add(Student1);
    await context.SaveChangesAsync();
}

  catch (DbUpdateException ex){
    Console.WriteLine(ex.GetType().Name);
}


var hamdy = await context.Instructors.FirstAsync(i => i.FullName == "Hamdy");

if (hamdy != null){
    var webCourse = await context.Courses.FirstAsync(c =>c.CourseName == "Web Development Using .NET");

    if (webCourse != null){
        webCourse.InstructorId = hamdy.Id;
        await context.SaveChangesAsync();
        Console.WriteLine("Course successfully assigned.");
    }
}
try
{
    var invalidCourse = new Course{
        CourseName = "Algorithms",
        Credits = 3,
        InstructorId = 9999
    };

    context.Courses.Add(invalidCourse);
    await context.SaveChangesAsync();
}
catch (Exception ex){
    Console.WriteLine(ex.Message);
}


hamdy = await context.Instructors.FirstAsync(i => i.FullName == "Hamdy");

if (hamdy != null){
    var ExtraCourses = new List<Course>{
        new Course{
            CourseName = "Course One",
            Credits = 3,
            InstructorId = hamdy.Id
        },

        new Course{
            CourseName = "Course Two",
            Credits = 4,
            InstructorId = hamdy.Id
        }
    };

    context.Courses.AddRange(ExtraCourses);
    await context.SaveChangesAsync();
}



var allInstructors = await context.Instructors.ToListAsync();

foreach (var instructor in allInstructors){
    Console.WriteLine(instructor.FullName);
    Console.WriteLine(instructor.Courses.Count);
}

allInstructors = await context.Instructors
    .Include(i => i.Courses)
    .ToListAsync();

foreach (var instructor in allInstructors){
    Console.WriteLine(instructor.FullName);

    foreach (var course in instructor.Courses){
        Console.WriteLine(course.CourseName);
    }
}

var oneInstructor = await context.Instructors.FirstAsync();
Console.WriteLine(oneInstructor.Courses.Count);

await context.Entry(oneInstructor)
    .Collection(i => i.Courses)
    .LoadAsync();

Console.WriteLine(oneInstructor.Courses.Count);

var AllStudents = await context.Students
    .AsNoTracking()
    .ToListAsync();

AllStudents[0].Gpa = 4.0;

await context.SaveChangesAsync();


    public class Student{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? FullName { get; set; }

    public int YearOfStudy { get; set; }

    public double Gpa { get; set; }

    public string? Email { get; set; }
}

public class Instructor{
    public int? Id { get; set; }

    public string FullName { get; set; }

    public int YearsOfExperience { get; set; }

    public List<Course> Courses { get; set; } = new();
}

public class Course{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string CourseName { get; set; }

    public int Credits { get; set; }

    public int? InstructorId { get; set; }

    public Instructor? Instructor { get; set; }
}


public class StudentPortalContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Instructor> Instructors => Set<Instructor>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite("Data Source=StudentPortal.db")
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging();
    }


protected override void OnModelCreating(ModelBuilder modelBuilder){
    modelBuilder.Entity<Student>()
    .Property(s => s.FullName)
    .IsRequired()
    .HasMaxLength(100);


    modelBuilder.Entity<Course>()
        .HasOne(c => c.Instructor)
        .WithMany(i => i.Courses)
        .HasForeignKey(c => c.InstructorId)
        .OnDelete(DeleteBehavior.SetNull);
    
}
}


