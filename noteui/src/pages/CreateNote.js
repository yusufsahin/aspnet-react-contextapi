import { useNavigate } from 'react-router-dom';
import { useNotes } from '../context/NotesContext';
import NoteForm from '../components/NoteForm';

const CreateNote = () => {
  const { addNote } = useNotes();
  const navigate = useNavigate();

  const handleSubmit = (data) => {
    addNote({ ...data, id: Date.now() });
    navigate('/');
  };

  return (
    <div>
      <h1>Create Note</h1>
      <NoteForm onSubmit={handleSubmit} />
    </div>
  );
};

export default CreateNote;
