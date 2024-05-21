namespace SmartSchool.WebAPI.Models;

public class AlunoDisciplina
{
    public AlunoDisciplina() { }

    public AlunoDisciplina(int alunoId, int disciplinaId)
    {
        AlunoId = alunoId;        
        DisciplinaId = disciplinaId;        
    }

    public int AlunoId { get; set; }
    public Aluno Aluno { get; set; }
    public int DisciplinaId { get; set; }
    public Disciplina Disciplina { get; set; }
}
