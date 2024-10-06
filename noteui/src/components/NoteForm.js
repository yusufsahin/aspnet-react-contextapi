import { useEffect } from 'react';
import { useNotes } from '../context/NotesContext';
import { useNavigate, useParams } from 'react-router-dom';
import { Button, Form } from 'react-bootstrap';
import { useForm, Controller } from 'react-hook-form';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';

// Define validation schema using Yup
const schema = yup.object().shape({
  name: yup.string().required('Name is required'),
  description: yup.string().required('Description is required'),
});

const NoteForm = () => {
  const { addNote, updateNote, notes } = useNotes();
  const { id } = useParams(); // Get the note id from the URL
  const navigate = useNavigate();

  // Initialize react-hook-form
  const {
    control,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema), // Attach validation schema
    defaultValues: {
      name: '',
      description: '',
    },
  });

  // If an id is present, find the existing note and set the form values
  useEffect(() => {
    if (id) {
      const existingNote = notes.find((n) => n.id === parseInt(id)); // Find the note by id
      if (existingNote) {
        setValue('name', existingNote.name); // Set name for editing
        setValue('description', existingNote.description); // Set description for editing
      }
    }
  }, [id, notes, setValue]);

  // Form submission handler
  const onSubmit = (data) => {
    if (id) {
      // If we are editing an existing note
      updateNote({ id: parseInt(id), ...data });
    } else {
      // If we are creating a new note
      addNote(data);
    }
    navigate('/'); // Redirect back to the notes list after submitting
  };

  return (
    <Form onSubmit={handleSubmit(onSubmit)}>
      <Form.Group controlId="name">
        <Form.Label>Name</Form.Label>
        <Controller
          name="name"
          control={control}
          render={({ field }) => (
            <Form.Control
              {...field}
              type="text"
              isInvalid={!!errors.name}
              placeholder="Enter note name"
            />
          )}
        />
        {errors.name && <Form.Control.Feedback type="invalid">{errors.name.message}</Form.Control.Feedback>}
      </Form.Group>

      <Form.Group controlId="description" className="mt-3">
        <Form.Label>Description</Form.Label>
        <Controller
          name="description"
          control={control}
          render={({ field }) => (
            <Form.Control
              {...field}
              as="textarea"
              isInvalid={!!errors.description}
              placeholder="Enter note description"
            />
          )}
        />
        {errors.description && <Form.Control.Feedback type="invalid">{errors.description.message}</Form.Control.Feedback>}
      </Form.Group>

      <Button variant="primary" type="submit" className="mt-3">
        {id ? 'Update Note' : 'Create Note'} {/* Button text changes based on create or edit mode */}
      </Button>
    </Form>
  );
};

export default NoteForm;
