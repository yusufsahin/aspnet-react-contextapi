import React ,{ useState } from 'react';
import { useNotes } from '../context/NotesContext';
import { Table, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom'; // Import Link for navigation
import ConfirmDialog from '../components/ConfirmDialog';

const Notes = () => {
  const { notes, loading, error, deleteNote } = useNotes();
  const [showDialog, setShowDialog] = useState(false);
  const [selectedNoteId, setSelectedNoteId] = useState(null);

  const handleDeleteClick = (id) => {
    setSelectedNoteId(id);
    setShowDialog(true);
  };

  const handleConfirmDelete = () => {
    deleteNote(selectedNoteId);
    setShowDialog(false);
    setSelectedNoteId(null);
  };

  const handleCloseDialog = () => setShowDialog(false);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <div>
      <h1>Notes</h1>
      <Button
                  as={Link}
                  to={"/create"} // Navigate to the edit page
                  variant="secondary"
                  className="me-2"
                >
                  Create
                </Button>
                <hr/>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {notes.map((note) => (
            <tr key={note.id}>
              <td>{note.name}</td>
              <td>{note.description}</td>
              <td>
                {/* Edit Button */}
                <Button
                  as={Link}
                  to={`/edit/${note.id}`} // Navigate to the edit page
                  variant="warning"
                  className="me-2"
                >
                  Edit
                </Button>

                {/* Delete Button */}
                <Button variant="danger" onClick={() => handleDeleteClick(note.id)}>
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      {/* Confirmation Dialog for Deletion */}
      <ConfirmDialog
        show={showDialog}
        handleClose={handleCloseDialog}
        handleConfirm={handleConfirmDelete}
        message="Are you sure you want to delete this note?"
      />
    </div>
  );
};

export default Notes;
