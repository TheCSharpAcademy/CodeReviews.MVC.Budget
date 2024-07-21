import * as app from "./app.js";

attachExportedModuleMembersToWindow();

/**
 * When using <script type="module">, the functions aren't globally available and therefore
 * cannot be used with HTML `onclick` / `onsubmit` attributes. 
 * 
 * To make the above possible, all exported members of the `app` module
 * are being exposed directly on the global `window` object.
 * 
 * This isn't necessarily unsafe since only the explicitly exported members will be accessible.
 * 
 * Arguably we could namespace under a special key e.g. `window.__App.[exportedMemberName]` to
 * avoid collisions but it doesn't seem necessary due to the low complexity of the application
 * & low chance of custom name collision.
 */
function attachExportedModuleMembersToWindow() {
    Object.keys(app).forEach((exportedMemberName) => {
        window[exportedMemberName] = app[exportedMemberName];
    });
}
