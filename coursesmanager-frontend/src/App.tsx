import { useEffect, useState } from "react";
import type { Course } from "./types/course";
import { getCourses, updateCourse } from "./api/client";

export default function App() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [error, setError] = useState<string | null>(null);

  const [editing, setEditing] = useState<Course | null>(null);
  const [editTitle, setEditTitle] = useState("");
  const [editDescription, setEditDescription] = useState("");
  const [editError, setEditError] = useState<string | null>(null);

  useEffect(() => {
    loadCourses();
  }, []);

  async function loadCourses() {
    try {
      const data = await getCourses();
      setCourses(data);
    } catch (e) {
      setError(e instanceof Error ? e.message : "Kunde inte hämta kurser.");
    }
  }

  function startEdit(c: Course) {
    if (!c.courseCode) return; // skydd mot tom courseCode

    setEditing(c);
    setEditTitle(c.title);
    setEditDescription(c.description);
    setEditError(null);
  }

  function cancelEdit() {
    setEditing(null);
    setEditTitle("");
    setEditDescription("");
    setEditError(null);
  }

  async function saveEdit() {
    if (!editing) return;

    try {
      setEditError(null);

      await updateCourse(editing.courseCode, {
        courseCode: editing.courseCode,
        title: editTitle,
        description: editDescription,
        rowVersion: editing.rowVersion,
      });

      await loadCourses();
      setEditing(null);
    } catch (e) {
      setEditError(
        e instanceof Error ? e.message : "Kunde inte uppdatera kursen."
      );
    }
  }

  return (
    <div style={{ padding: 24, fontFamily: "system-ui" }}>
      <h1>Courses</h1>

      {error && <p style={{ color: "crimson" }}>Fel: {error}</p>}

      {!error && courses.length === 0 && <p>No courses found.</p>}

      <ul>
        {courses.map((c) => (
          <li key={c.id} style={{ marginBottom: 16 }}>
            <strong>{c.courseCode}</strong> — {c.title}
            <div style={{ opacity: 0.8 }}>{c.description}</div>

            {editing?.id === c.id ? (
              <div
                style={{
                  border: "1px solid #ccc",
                  padding: 12,
                  marginTop: 8,
                }}
              >
                <div>
                  <label>Titel</label>
                  <br />
                  <input
                    value={editTitle}
                    onChange={(e) => setEditTitle(e.target.value)}
                  />
                </div>

                <div style={{ marginTop: 8 }}>
                  <label>Beskrivning</label>
                  <br />
                  <input
                    value={editDescription}
                    onChange={(e) =>
                      setEditDescription(e.target.value)
                    }
                  />
                </div>

                {editError && (
                  <p style={{ color: "crimson", marginTop: 8 }}>
                    {editError}
                  </p>
                )}

                <div style={{ marginTop: 8 }}>
                  <button onClick={saveEdit}>Save</button>
                  <button
                    onClick={cancelEdit}
                    style={{ marginLeft: 8 }}
                  >
                    Cancel
                  </button>
                </div>
              </div>
            ) : !c.courseCode ? (
              <span
                style={{
                  color: "crimson",
                  display: "inline-block",
                  marginTop: 8,
                }}
              >
                Kan inte uppdatera (courseCode saknas)
              </span>
            ) : (
              <button
                onClick={() => startEdit(c)}
                style={{ marginTop: 8 }}
              >
                Edit
              </button>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
