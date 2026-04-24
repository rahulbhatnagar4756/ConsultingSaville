window.downloadFileFromBytes = (fileName, base64, contentType) => {
    const link = document.createElement('a');
    link.href = `data:${contentType};base64,${base64}`;
    link.download = fileName;
    link.click();
};
