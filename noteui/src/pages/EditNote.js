import { useNavigate, useParams } from 'react-router-dom';
import { useNotes } from '../context/NotesContext';
import NoteForm from '../components/NoteForm';

const EditNote = () => {
  const { id } = useParams();
  const { notes, updateNote } = useNotes();
  const navigate = useNavigate();
  
  const note = notes.find((n) => n.id === parseInt(id));

  const handleSubmit = (data) => {
    updateNote({ ...data, id: note.id });
    navigate('/');
  };

  return (
    <div>
      <h1>Edit Note</h1>
      <NoteForm onSubmit={handleSubmit} defaultValues={note} />
    </div>
  );
};

export default EditNote;
