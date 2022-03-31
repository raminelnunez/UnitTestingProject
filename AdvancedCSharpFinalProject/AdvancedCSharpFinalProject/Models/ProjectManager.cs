namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectManager : Developer //inherits from Developer
                                            //because I think that to actualy be a Project Manager you atleast have to be a proper Developer
    {
        public double Budget { get; set; }
    }
}
