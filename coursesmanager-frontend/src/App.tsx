import { useEffect, useMemo, useState } from "react";
import type { Course } from "./types/course";
import { getCourses, updateCourse, createCourse } from "./api/client";
import { styles } from "./styles/appStyles";

type UpdateCourseDto = {
  courseCode: string;
  title: string;
  description: string;
  rowVersion: string;
};

export default function App() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const [editing, setEditing] = useState<Course | null>(null);
  const [editTitle, setEditTitle] = useState("");
  const [editDescription, setEditDescription] = useState("");
  const [editError, setEditError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);

  const [query, setQuery] = useState("");

  useEffect(() => {
    loadCourses();
  }, []);

  async function loadCourses() {
    try {
      setLoading(true);
      setError(null);
      const data = await getCourses();
      setCourses(data);
    } catch (e) {
      setError(e instanceof Error ? e.message : "Kunde inte hämta kurser.");
    } finally {
      setLoading(false);
    }
  }

  async function createDemoCourse() {
    try {
      setError(null);

      // Enkel slumpkod så du inte får conflict varje gång
      const code = `WEB${Math.floor(Math.random() * 900 + 100)}`; // WEB123

      await createCourse({
        courseCode: code,
        title: "Ny kurs",
        description: "Skapad från frontend",
      });

      await loadCourses();
    } catch (e) {
      setError(e instanceof Error ? e.message : "Kunde inte skapa kursen.");
    }
  }

  function startEdit(c: Course) {
    if (!c.courseCode) return;

    setEditing(c);
    setEditTitle(c.title ?? "");
    setEditDescription(c.description ?? "");
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

    const title = editTitle.trim();
    const description = editDescription.trim();

    if (!title) {
      setEditError("Titel kan inte vara tom.");
      return;
    }

    try {
      setSaving(true);
      setEditError(null);

      const payload: UpdateCourseDto = {
        courseCode: editing.courseCode,
        title,
        description,
        rowVersion: editing.rowVersion,
      };

      await updateCourse(editing.courseCode, payload);

      await loadCourses();
      setEditing(null);
    } catch (e) {
      setEditError(
        e instanceof Error ? e.message : "Kunde inte uppdatera kursen."
      );
    } finally {
      setSaving(false);
    }
  }

  const filteredCourses = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return courses;

    return courses.filter((c) => {
      const code = (c.courseCode ?? "").toLowerCase();
      const title = (c.title ?? "").toLowerCase();
      const desc = (c.description ?? "").toLowerCase();
      return code.includes(q) || title.includes(q) || desc.includes(q);
    });
  }, [courses, query]);

  const stats = useMemo(() => {
    const total = courses.length;
    const missingCode = courses.filter((c) => !c.courseCode).length;
    return { total, missingCode };
  }, [courses]);

  return (
    <div style={styles.page}>
      <div style={styles.bgGlowTop} />
      <div style={styles.bgGlowBottom} />

      <div style={styles.container}>
        <header style={styles.header}>
          <div>
            <div style={styles.kicker}>Education Admin</div>
            <h1 style={styles.h1}>Courses</h1>
            <p style={styles.sub}>
              Hantera kurskatalogen. Klicka <b>Edit</b> för att uppdatera titel
              och beskrivning.
            </p>
          </div>

          <div style={styles.headerRight}>
            <div style={styles.statCard}>
              <div style={styles.statLabel}>Totalt</div>
              <div style={styles.statValue}>{stats.total}</div>
            </div>

            <div style={styles.statCard}>
              <div style={styles.statLabel}>Saknar courseCode</div>
              <div style={styles.statValue}>{stats.missingCode}</div>
            </div>
          </div>
        </header>

        <section style={styles.toolbar}>
          <div style={styles.searchWrap}>
            <span style={styles.searchIcon}>⌕</span>
            <input
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              placeholder="Sök på kurskod, titel eller beskrivning…"
              style={styles.searchInput}
            />
          </div>

          <button
            onClick={loadCourses}
            style={{ ...styles.button, ...styles.buttonGhost }}
            disabled={loading}
            title="Hämta om från API"
          >
            {loading ? "Refreshing…" : "Refresh"}
          </button>

          <button
            onClick={createDemoCourse}
            style={{ ...styles.button, ...styles.buttonPrimary }}
            disabled={loading}
            title="Skapar en kurs via POST från frontend"
          >
            Create demo
          </button>
        </section>

        {error && (
          <div style={styles.alertError}>
            <div style={styles.alertTitle}>Något gick fel</div>
            <div style={styles.alertText}>{error}</div>
          </div>
        )}

        {!error && loading && courses.length === 0 && (
          <div style={styles.centerText}>Laddar kurser…</div>
        )}

        {!error && !loading && filteredCourses.length === 0 && (
          <div style={styles.centerText}>Inga kurser matchar din sökning.</div>
        )}

        <div style={styles.grid}>
          {filteredCourses.map((c) => {
            const isEditing = editing?.id === c.id;
            const canEdit = Boolean(c.courseCode);

            return (
              <article key={c.id} style={styles.card}>
                <div style={styles.cardTop}>
                  <div style={styles.badgeRow}>
                    <span style={styles.badge}>
                      {c.courseCode ? c.courseCode : "NO CODE"}
                    </span>
                    {!c.courseCode && (
                      <span style={styles.badgeWarn}>Kan inte uppdatera</span>
                    )}
                  </div>

                  <h3 style={styles.cardTitle}>{c.title}</h3>
                  <p style={styles.cardDesc}>
                    {c.description || (
                      <span style={{ opacity: 0.7 }}>
                        Ingen beskrivning ännu.
                      </span>
                    )}
                  </p>
                </div>

                <div style={styles.cardBottom}>
                  {isEditing ? (
                    <div style={styles.editor}>
                      <div style={styles.field}>
                        <label style={styles.label}>Titel</label>
                        <input
                          value={editTitle}
                          onChange={(e) => setEditTitle(e.target.value)}
                          style={styles.input}
                          placeholder="t.ex. Databaser 1"
                        />
                      </div>

                      <div style={styles.field}>
                        <label style={styles.label}>Beskrivning</label>
                        <textarea
                          value={editDescription}
                          onChange={(e) => setEditDescription(e.target.value)}
                          style={styles.textarea}
                          placeholder="Kort beskrivning av kursen…"
                          rows={3}
                        />
                      </div>

                      {editError && (
                        <div style={styles.inlineError}>
                          <span style={styles.inlineErrorDot} />
                          <span>{editError}</span>
                        </div>
                      )}

                      <div style={styles.actions}>
                        <button
                          onClick={saveEdit}
                          style={{ ...styles.button, ...styles.buttonPrimary }}
                          disabled={saving}
                        >
                          {saving ? "Saving…" : "Save"}
                        </button>
                        <button
                          onClick={cancelEdit}
                          style={{ ...styles.button, ...styles.buttonGhost }}
                          disabled={saving}
                        >
                          Cancel
                        </button>
                      </div>

                      <div style={styles.tinyHint}>
                        Tips: om du får concurrency-fel (rowVersion) – uppdatera
                        listan och försök igen.
                      </div>
                    </div>
                  ) : (
                    <button
                      onClick={() => startEdit(c)}
                      style={{
                        ...styles.button,
                        ...(canEdit ? styles.buttonPrimary : styles.buttonDisabled),
                      }}
                      disabled={!canEdit}
                    >
                      Edit
                    </button>
                  )}
                </div>
              </article>
            );
          })}
        </div>

        <footer style={styles.footer}>&copy; {new Date().getFullYear()}</footer>
      </div>
    </div>
  );
}
