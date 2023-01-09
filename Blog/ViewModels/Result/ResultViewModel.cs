namespace Blog.ViewModels.Result
{
    public class ResultViewModel<T>
    {   
        // sucesso e erros
        public ResultViewModel(T data, List<string> errors){
            Data = data;
            Errors = errors;
        }
        
        // sucesso
        public ResultViewModel(T data)
        {
            Data = data;
        }
        
        // lista error
        public ResultViewModel(List<string> errors)
        {
            Errors = errors;
        }

        // quando tenho somente um erro, adiciono o erro na lista de erros
        public ResultViewModel(string erro)
        {
            Errors.Add(erro);
        }

        public T? Data { get; private set; }
        public List<string>? Errors { get; private set; } = new ();
    }
}